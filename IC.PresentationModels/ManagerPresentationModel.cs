using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
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
		private readonly IUnityContainer _container;
		private readonly IProjectProcesses _projectProcesses;

		private IProject _currentProject;
		
		
		#region Methods for handling subscribed events

		private void OnProjectCreating(EventArgs args)
		{
			if (!_currentProject.IsSaved)
			{
				var result = MessageBox.Show("Текущий проект не сохранён, создание нового проекта приведёт к закрытию текущего.\r\nСохранить изменения перед закрытием проекта?",
											 "Сохранение",
											 MessageBoxButton.YesNoCancel,
											 MessageBoxImage.Question);
				switch (result)
				{
					case MessageBoxResult.Yes:
						_projectProcesses.Save(_currentProject);
						break;
					case MessageBoxResult.Cancel:
						return;
				}
			}
            
			_container.Resolve<ICreateProjectWindow>().ShowDialog();
		}

		private void OnProjectSaving(EventArgs args)
		{
			ProcessResult<List<ISchema>> result = _projectProcesses.Save(_currentProject);
			if (result.NoErrors)
			{
				_eventAggregator.GetEvent<ProjectSavedEvent>().Publish(EventArgs.Empty);
			}
			else
			{
				if (result.Result != null)
				{
					_eventAggregator.GetEvent<SchemaSavedEvent>().Subscribe(OnSchemaSavedEvent);
					_eventAggregator.GetEvent<SchemaSavingEvent>().Publish(null);
				}
			}
		}

		private void OnSchemaSavedEvent(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaSavedEvent>().Unsubscribe(OnSchemaSavedEvent);
			ProcessResult<List<ISchema>> result = _projectProcesses.Save(_currentProject);
			if (result.NoErrors)
			{
				_eventAggregator.GetEvent<ProjectSavedEvent>().Publish(EventArgs.Empty);
			}
		}

		#endregion

		public ManagerPresentationModel([NotNull] IEventAggregator eventAggregator,
										[NotNull] IUnityContainer container,
										[NotNull] IProjectProcesses projectProcesses)
			: base(eventAggregator)
		{
			_container = container;
			_projectProcesses = projectProcesses;

			_eventAggregator.GetEvent<ProjectCreatingEvent>().Subscribe(OnProjectCreating);
			_eventAggregator.GetEvent<ProjectSavingEvent>().Subscribe(OnProjectSaving);
		}
	}
}
