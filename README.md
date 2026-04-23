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
        │   ├── MyLinkedList.cs      → Linked list implementation
        │   ├── MyLinkedListNode.cs  → Node for linked list
        │   └── LinkedListIterator.cs → Iterator for linked list
        │
        │── BST
        │   ├── MyBST.cs        → Binary Search Tree implementation
        │   ├── MyBSTNode.cs    → Node for BST
        │   └── BSTIterator.cs  → Iterator for BST
        │
        │── HashMap
        │   ├── MyHashMap.cs            → HashMap implementation
        │   ├── HashMapIterator.cs      → Iterator for HashMap
        │   └── TaskHashMapCollection.cs → Adapter for Task Manager

        ├── Model
        │   ├── TaskItem.cs            → Main task model
        │   ├── UserRole.cs            → User roles
        │   ├── TaskPriority.cs        → Priority enum
        │   ├── TaskState.cs           → Status enum
        │   ├── DataStructureType.cs   → Structure selection enum
        │   ├── RemoveTaskResult.cs    → Delete result enum
        │   └── AssignTaskResult.cs    → Assign result enum

        ├── Repository
        │   ├── ITaskRepository.cs     → Repository interface
        │   └── JsonTaskRepository.cs  → JSON load/save logic

        ├── Service
        │   ├── ITaskService.cs        → Service interface
        │   └── TaskService.cs         → Business logic layer

        ├── View
        │   ├── ITaskView.cs               → View interface
        │   ├── ConsoleTaskView.cs         → Main Kanban UI
        │   ├── TaskFilterMode.cs         → Status filter enum
        │   ├── TaskPriorityFilterMode.cs → Priority filter enum
        │   ├── TaskDateFilterMode.cs     → Date filter enum
        │   └── TaskSortMode.cs           → Sorting enum

        ├── PhaseDemos
        │   ├── DynamicArrayDemo.cs → Demo for array list
        │   ├── LinkedListDemo.cs   → Demo for linked list
        │   ├── BSTDemo.cs          → Demo for BST
        │   └── HashMapDemo.cs      → Demo for HashMap

        └── Tests
            ├── TestRunner.cs       → Runs all tests
            ├── TestHelper.cs       → PASS/FAIL output helper
            ├── ArrayListTests.cs   → Tests for array list
            ├── LinkedListTests.cs  → Tests for linked list
            ├── BSTTests.cs         → Tests for BST
            └── HashMapTests.cs     → Tests for HashMap