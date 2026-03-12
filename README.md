# DSA-P1 — Task Management Console Application

# Project Overview

This project is a console-based task management system developed in C#.

The main goal of the application is to manage tasks using a layered architecture while gradually replacing built-in collection classes with custom data structure implementations.

The system allows users to:

- Create tasks

- Remove tasks

- Update tasks

- View tasks

In later stages, advanced features such as filtering, sorting, dependency tracking, Kanban movement, and performance comparison between data structures will be implemented.

This project focuses not only on functionality but also on applying important software engineering principles such as:

1- Modular design

2- Abstraction

3- Separation of concerns

4- Maintainability

5- Scalability

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

# Architecture Overview

The application follows a layered architecture, consisting of the following layers:

- Model Layer

- Repository Layer

- Service Layer

- View Layer

- DataStructures Layer

Each layer has a clearly defined responsibility, which improves:

- Code readability

- Maintainability

- Testability

- Flexibility

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

# Project Folder Structure

📁 Model

The Model folder contains domain entities that represent real-world objects used by the application.

🎯 Purpose

To store structured data related to tasks.

📄 Current Content

TaskItem.cs

This class represents a task and includes basic properties such as:

Task identifier

Task description

Completion status

Future enhancements may include:

Task priority

Task status categories

Task assignment

Task dependency tracking

--------------------------------------------------------------------------------------------------------

📁 Repository

The Repository folder is responsible for handling data persistence.

🎯 Purpose

To abstract how tasks are stored and retrieved.

This ensures that the rest of the application does not depend on a specific storage mechanism.

📄 Files

ITaskRepository.cs
Defines the contract for loading and saving tasks.

JsonTaskRepository.cs
Implements the repository using a JSON file as storage.
This enables persistence between application runs.

Future implementations may include:

Database storage

Alternative file formats

--------------------------------------------------------------------------------------------------------

📁 Service

The Service folder contains the business logic of the application.

🎯 Purpose

To control how tasks are created, modified, and removed.

This layer acts as a bridge between:

Repository (data storage)

View (user interface)

📄 Files

ITaskService.cs
Defines operations that can be performed on tasks.

TaskService.cs
Implements task management logic such as:

Creating tasks

Removing tasks

Updating completion state

Communicating with the repository

--------------------------------------------------------------------------------------------------------

📁 View

The View folder manages interaction with the user.

🎯 Purpose

To display information and capture user input through the console interface.

📄 Files

ITaskView.cs
Defines the user interface contract.

ConsoleTaskView.cs
Implements a console-based menu system allowing users to:

View tasks

Add tasks

Remove tasks

Toggle completion

Future updates will include:

Filtering

Sorting

Kanban-style task movement

--------------------------------------------------------------------------------------------------------

📁 DataStructures

The DataStructures folder contains custom implementations of collection types.

The project requirement specifies that built-in collections will eventually be replaced with self-implemented data structures.

--------------------------------------------------------------------------------------------------------

📁 Interfaces

Contains generic collection interfaces:

IMyCollection.cs → defines operations such as add, remove, and iteration

IMyIterator.cs → defines traversal logic

These interfaces allow different data structures to be used interchangeably.

--------------------------------------------------------------------------------------------------------

📁 ArrayList

Will contain a dynamic array implementation.

Planned responsibilities:

Automatic resizing

Element insertion and removal

Iterator support

This structure will be the first replacement for built-in lists.

--------------------------------------------------------------------------------------------------------

📁 LinkedList

Will contain a custom linked list implementation.

Planned use cases:

Sequential traversal

Alternative storage strategy

Performance comparison

--------------------------------------------------------------------------------------------------------

📁 HashMap

Will contain a custom hash table implementation.

Planned use cases:

Fast lookup by key

Task dependency management

Efficient search operations

--------------------------------------------------------------------------------------------------------

📁 BST (Binary Search Tree)

Will contain a binary search tree implementation.

Planned use cases:

Sorted task storage

Range queries

Optimized filtering

--------------------------------------------------------------------------------------------------------

📄 Program.cs

This file serves as the application entry point.

Responsibilities include:

Creating repository instance

Creating service instance

Creating view instance

Starting the application loop

This file connects all layers together.

--------------------------------------------------------------------------------------------------------

📄 tasks.json

This file stores serialized task data.

It allows the application to persist tasks between sessions.