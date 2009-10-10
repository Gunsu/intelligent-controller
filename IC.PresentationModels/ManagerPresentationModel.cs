using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Manager;
using IC.UI.Infrastructure.Interfaces.Windows;
using Project.Utils.Common;
using ValidationAspects;
using ValidationAspects.PostSharp;
using System.Windows;
using IC.CoreInterfaces.Processes;

namespace IC.PresentationModels
{
	/// <summary>
	/// PresentationModel для менеджера главного окна.
	/// Менеджер отслеживает и обрабатывает глобальные события
	/// такие, как создание проекта, сохранение проекта, закрытие приложения и т. д.
	/// </summary>
	[Validate]
	public sealed class ManagerPresentationModel : BasePresentationModel, IManagerPresentationModel
	{
		private readonly IProjectProcesses _projectProcesses;
		private readonly ICreateProjectWindow _createProjectWindow;
		private readonly ICreateSchemaWindow _createSchemaWindow;

		public IProject CurrentProject;
		
		
		#region Methods for handling subscribed events

		private void OnProjectCreating(EventArgs args)
		{
			if ((CurrentProject != null) && (!CurrentProject.IsSaved))
			{
				var result = MessageBox.Show("Текущий проект не сохранён, создание нового проекта приведёт к закрытию текущего.\r\nСохранить изменения перед закрытием проекта?",
											 "Сохранение",
											 MessageBoxButton.YesNoCancel,
											 MessageBoxImage.Question);
				switch (result)
				{
					case MessageBoxResult.Yes:
						_projectProcesses.Save(CurrentProject);
						break;
					case MessageBoxResult.Cancel:
						return;
				}
			}

			_createProjectWindow.ShowDialog();
		}

		private void OnProjectSaving(EventArgs args)
		{
			ProcessResult<List<ISchema>> result = _projectProcesses.Save(CurrentProject);
			if (result.NoErrors)
			{
				_eventAggregator.GetEvent<ProjectSavedEvent>().Publish(EventArgs.Empty);
			}
			else
			{
				if (result.Result != null)
				{
					_eventAggregator.GetEvent<SchemaSavedEvent>().Subscribe(OnSchemaSaved);
					_eventAggregator.GetEvent<SchemaSavingEvent>().Publish(null);
				}
			}
		}

		private void OnSchemaCreating(EventArgs args)
		{
			_createSchemaWindow.ShowDialog(CurrentProject);
		}

		private void OnSchemaSaved(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaSavedEvent>().Unsubscribe(OnSchemaSaved);
			ProcessResult<List<ISchema>> result = _projectProcesses.Save(CurrentProject);
			if (result.NoErrors)
			{
				_eventAggregator.GetEvent<ProjectSavedEvent>().Publish(EventArgs.Empty);
			}
		}

		private void OnProjectCreated(IProject project)
		{
			CurrentProject = project;
		}

		#endregion

		public ManagerPresentationModel([NotNull] IEventAggregator eventAggregator,
										[NotNull] IProjectProcesses projectProcesses,
										[NotNull] ICreateProjectWindow createProjectWindow,
										[NotNull] ICreateSchemaWindow createSchemaWindow)
			: base(eventAggregator)
		{
			_projectProcesses = projectProcesses;
			_createProjectWindow = createProjectWindow;
			_createSchemaWindow = createSchemaWindow;

			_eventAggregator.GetEvent<ProjectCreatingEvent>().Subscribe(OnProjectCreating);
			_eventAggregator.GetEvent<ProjectSavingEvent>().Subscribe(OnProjectSaving);
			_eventAggregator.GetEvent<ProjectCreatedEvent>().Subscribe(OnProjectCreated);
			_eventAggregator.GetEvent<SchemaCreatingEvent>().Subscribe(OnSchemaCreating);
		}
	}
}
