import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoService, Todo } from '../../core/todo.service';

@Component({
  standalone: true,
  selector: 'app-todo-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-page.component.html'
})
export class TodoPageComponent implements OnInit {

  todos: Todo[] = [];
  nuevoTitulo = '';
  loading = false;

  constructor(private todoService: TodoService) {}

  ngOnInit() {
    this.cargarTodos();
  }

  cargarTodos() {
    this.todoService.getTodos().subscribe(t => this.todos = t);
  }

  agregar() {
    if (!this.nuevoTitulo.trim()) return;

    this.loading = true;
    this.todoService.addTodo(this.nuevoTitulo).subscribe(todo => {
      this.todos.push(todo);
      this.nuevoTitulo = '';
      this.loading = false;
    });
  }

  toggle(todo: Todo) {
    todo.completado = !todo.completado;
    this.todoService.updateTodo(todo).subscribe();
  }

  eliminar(id: number) {
    this.todoService.deleteTodo(id).subscribe(() => {
      this.todos = this.todos.filter(t => t.id !== id);
    });
  }

  get total() {
    return this.todos.length;
  }

  get completadas() {
    return this.todos.filter(t => t.completado).length;
  }

  get pendientes() {
    return this.todos.filter(t => !t.completado).length;
  }
}
