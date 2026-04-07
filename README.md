# DSA-P1 — Task Management Console Application

# Project Overview

This project is a console-based task management system developed in C# using a layered architecture and custom data structures.

The goal of the project is to replace built-in collections with self-implemented data structures while building a functional and scalable task management system.

The application allows users to:

- Create tasks  
- Remove tasks  
- Update tasks  
- Assign tasks to users  
- Manage task dependencies  
- Filter and sort tasks  
- View tasks in a Kanban-style board  

Additionally, the system supports:

- Role-based access (Project Manager / Worker)  
- Task assignment and ownership  
- Permission enforcement  
- Multiple custom data structures  

---

# Key Features

## Task Management
- Full CRUD operations
- Task priority (Low, Medium, High)
- Task status (Todo, InProgress, Done)

## Filtering & Sorting
- Filter by status
- Filter by priority
- Filter by creation date
- Sort by ID, description, and date

## Task Dependencies
- Tasks can depend on other tasks
- Cannot mark task as Done if dependencies are incomplete
- Circular dependency prevention

## Role-Based System
- Project Manager:
  - Can modify all tasks
  - Can assign tasks
- Worker:
  - Can only modify assigned tasks
- UI shows clear error messages for unauthorized actions

## Assignment System
- Tasks can be assigned to users
- Assigned user is displayed in UI

---

# Architecture Overview

The application follows a layered architecture:

- Model Layer → Data representation  
- Repository Layer → Data persistence  
- Service Layer → Business logic  
- View Layer → Console UI  
- DataStructures Layer → Custom structures  

This ensures:

- Separation of concerns  
- Maintainability  
- Scalability  
- Flexibility  

---

# Project Folder Structure

## 📁 Model
Contains domain objects:

- TaskItem (task data + dependencies + assignment)
- Enums (TaskState, TaskPriority, UserRole)

---

## 📁 Repository
Handles data storage:

- ITaskRepository → contract
- JsonTaskRepository → JSON persistence

---

## 📁 Service
Contains core logic:

- TaskService
  - CRUD operations
  - Dependency management
  - Role-based permissions
  - Assignment logic
  - Uses HashMap for fast lookup

---

## 📁 View
Console interface using Spectre.Console:

- Kanban board visualization
- Filtering & sorting UI
- Role-based interaction
- Permission-aware actions

---

## 📁 DataStructures

Custom implementations replacing built-in collections:

### ArrayList
- Dynamic array
- Automatic resizing
- Iterator support

### LinkedList
- Node-based structure
- Efficient insert/remove
- Sequential traversal

### HashMap
- Custom hash table
- Key → value mapping
- O(1) lookup for tasks by ID
- Used in TaskService

### Binary Search Tree (BST)
- Hierarchical structure
- Ordered storage
- Traversal (InOrder, PreOrder, PostOrder)
- Used for demonstration and comparison

---

## 📁 PhaseDemos
Demonstrates each data structure:

- Dynamic Array Demo  
- Linked List Demo  
- HashMap Demo  
- BST Demo  

---

## 📄 Program.cs
Application entry point:

- Menu system
- Demo selection
- Role selection (Manager / Worker)
- Starts Task Manager

---

## 📄 tasks.json
Stores task data persistently.

---

# Data Structures Used

| Structure   | Purpose |
|------------|--------|
| ArrayList  | Dynamic storage |
| LinkedList | Alternative sequential storage |
| HashMap    | Fast lookup by ID (O(1)) |
| BST        | Sorted traversal and hierarchy |

---

# Conclusion

This project demonstrates:

- Implementation of core data structures
- Real-world application of DSA concepts
- Clean architecture design
- Performance optimization using HashMap
- Role-based access control

The system is scalable and can be extended with features like databases, authentication, or APIs.