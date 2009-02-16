#ifndef PROJECT_H
#define PROJECT_H

#include "BlockFactory.h"
#include "SchemaManager.h"
#include "Exceptions.h"
#include <string>

/*******************************
*          Project
********************************/

typedef void ((ProjectClosedCallback)());
typedef void ((ProjectOpenedCallback)(std::string projectFilePath));

class Project
{
	protected:
		static Project project;

	public:
		static Project* GetProject () { return &project; }

	protected:
		std::string projectFilePath;
		std::string blockFactoryFile;
		bool isLoad;
		ProjectClosedCallback* projectClosedCallback;
		ProjectOpenedCallback* projectOpenedCallback;

		void Close (bool callCallback);

	public:
		Project () : isLoad (false), projectClosedCallback (NULL), projectOpenedCallback (NULL) {}
		void Load (std::string projectFilePath, std::string componentsFolderPath);
		void Save ();
		void SaveAs (std::string projectFilePath);
		void New (std::string projectFilePath, std::string blockFactoryFilePath, std::string blockFactoryFile);
		bool IsLoad () { return isLoad; }

		void SetProjectClosedCallback (ProjectClosedCallback* projectClosedCallback);
		void SetProjectOpenedCallback (ProjectOpenedCallback* projectOpenedCallback);
};

#endif