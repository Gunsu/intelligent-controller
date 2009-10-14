using System;
using System.Collections.Generic;
using System.IO;
using IC.Core.Abstract;
using IC.Core.Entities;
using Microsoft.Practices.Composite.Events;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Manager;
using IC.UI.Infrastructure.Interfaces.Windows;
using Microsoft.Practices.Composite.Presentation.Events;
using Microsoft.Win32;
using ValidationAspects;
using ValidationAspects.PostSharp;
using System.Windows;

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
		private readonly IProjectsRepository _projectsRepository;
		private readonly ICreateProjectWindow _createProjectWindow;
		private readonly ICreateSchemaWindow _createSchemaWindow;

		public Project CurrentProject;
		
		
		#region Methods for handling subscribed events

		private void OnProjectCreated(Project project)
		{
			CurrentProject = project;
		}

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
						_projectsRepository.Update(CurrentProject);
						break;
					case MessageBoxResult.Cancel:
						return;
				}
			}

			_createProjectWindow.ShowDialog();
		}

		private void OnProjectOpening(EventArgs args)
		{
			var dialog = new OpenFileDialog();
			dialog.CheckPathExists = true;
			if (dialog.ShowDialog() == true)
			{
				var result = _projectsRepository.Load(Path.GetFileNameWithoutExtension(dialog.FileName));
				CurrentProject = result;
				_eventAggregator.GetEvent<ProjectOpenedEvent>().Publish(result);
			}
		}

		private void OnProjectSaving(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaSavingEvent>().Publish(null);
			_projectsRepository.Update(CurrentProject);
			_eventAggregator.GetEvent<ProjectSavedEvent>().Publish(EventArgs.Empty);
		}

		private void OnSchemaCreating(EventArgs args)
		{
			_createSchemaWindow.ShowDialog(CurrentProject);
		}

		#endregion

		public ManagerPresentationModel([NotNull] IEventAggregator eventAggregator,
										[NotNull] IProjectsRepository projectsRepository,
										[NotNull] ICreateProjectWindow createProjectWindow,
										[NotNull] ICreateSchemaWindow createSchemaWindow)
			: base(eventAggregator)
		{
			_projectsRepository = projectsRepository;
			_createProjectWindow = createProjectWindow;
			_createSchemaWindow = createSchemaWindow;

			_eventAggregator.GetEvent<ProjectCreatedEvent>().Subscribe(OnProjectCreated);
			_eventAggregator.GetEvent<ProjectCreatingEvent>().Subscribe(OnProjectCreating);
			_eventAggregator.GetEvent<ProjectOpeningEvent>().Subscribe(OnProjectOpening, ThreadOption.UIThread);
			_eventAggregator.GetEvent<ProjectSavingEvent>().Subscribe(OnProjectSaving);
			_eventAggregator.GetEvent<SchemaCreatingEvent>().Subscribe(OnSchemaCreating, ThreadOption.UIThread);
		}
	}
}
