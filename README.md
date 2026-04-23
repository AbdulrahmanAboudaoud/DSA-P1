# DSA-P1 — Task Management Console Application

## Overview

DSA-P1 is a C# console-based Task Management System created for the Data Structures & Algorithms course.  
The application allows users to manage tasks while using custom-built data structures instead of standard collections.

Users can:

- Add, remove, edit, and assign tasks
- Change task status and priority
- Add task dependencies
- View tasks in a Kanban board
- Filter and sort tasks
- Run the system using different data structures
- Run tests and demos for each structure

The project follows a layered architecture to keep the code clean, organized, and scalable.

---

# Student Contributions

## Abdulrahman

Responsible for:

- Implementing all custom data structures:
  - Dynamic Array
  - Linked List
  - Binary Search Tree
  - HashMap
- Creating shared interfaces (`IMyCollection`, `IMyIterator`)
- Integrating data structures into the Task Manager
- Building demos for each data structure
- Creating full console-based test system
- Improving menus and console UI
- Debugging and fixing structure logic

## Ibrahim

Responsible for:

- Core Task Manager functionality
- Task CRUD operations
- Service and repository layers
- JSON persistence system
- Task assignment system
- Permissions / roles system
- Task dependency logic
- Main application flow

---

# Project Structure

```text
DSA-P1
│── README.md                 → Project documentation
│── .gitignore                → Ignore build/temp files

└── DSA-P1-KH
    └── DSA-P1-KH

        │── Program.cs        → Main entry point and menu system
        │── tasks.json        → Stores all tasks in JSON
        │── DSA-P1-KH.csproj → Project file

        ├── DataStructures
        │
        │── Interfaces
        │   ├── IMyCollection.cs → Custom collection interface
        │   └── IMyIterator.cs   → Custom iterator interface
        │
        │── ArrayList
        │   ├── MyArrayList.cs   → Dynamic array implementation
        │   └── ArrayIterator.cs → Iterator for array list
        │
        │── LinkedList
        │   ├── MyLinkedList.cs       → Linked list implementation
        │   ├── MyLinkedListNode.cs   → Node class
        │   └── LinkedListIterator.cs → Iterator
        │
        │── BST
        │   ├── MyBST.cs        → Binary Search Tree
        │   ├── MyBSTNode.cs    → BST node
        │   └── BSTIterator.cs  → Iterator
        │
        │── HashMap
        │   ├── MyHashMap.cs             → HashMap implementation
        │   ├── HashMapIterator.cs       → Iterator
        │   └── TaskHashMapCollection.cs → Adapter for task system

        ├── Model
        │   ├── TaskItem.cs
        │   ├── UserRole.cs
        │   ├── TaskPriority.cs
        │   ├── TaskState.cs
        │   ├── DataStructureType.cs
        │   ├── RemoveTaskResult.cs
        │   └── AssignTaskResult.cs

        ├── Repository
        │   ├── ITaskRepository.cs
        │   └── JsonTaskRepository.cs

        ├── Service
        │   ├── ITaskService.cs
        │   └── TaskService.cs

        ├── View
        │   ├── ITaskView.cs
        │   ├── ConsoleTaskView.cs
        │   ├── TaskFilterMode.cs
        │   ├── TaskPriorityFilterMode.cs
        │   ├── TaskDateFilterMode.cs
        │   └── TaskSortMode.cs

        ├── PhaseDemos
        │   ├── DynamicArrayDemo.cs
        │   ├── LinkedListDemo.cs
        │   ├── BSTDemo.cs
        │   └── HashMapDemo.cs

        └── Tests
            ├── TestRunner.cs
            ├── TestHelper.cs
            ├── ArrayListTests.cs
            ├── LinkedListTests.cs
            ├── BSTTests.cs
            └── HashMapTests.cs