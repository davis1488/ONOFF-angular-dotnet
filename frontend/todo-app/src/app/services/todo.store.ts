import { Injectable, signal, computed } from '@angular/core';
import { Todo, TodoStats } from './todo.service';

@Injectable({ providedIn: 'root' })
export class TodoStore {
  private todosSignal = signal<Todo[]>([]);
  private statsSignal = signal<TodoStats>({ total: 0, completed: 0, pending: 0 });
  private filterSignal = signal<'all' | 'completed' | 'pending'>('all');
  private loadingSignal = signal<boolean>(false);
  
  // Señales de solo lectura
  todos = this.todosSignal.asReadonly();
  stats = this.statsSignal.asReadonly();
  filter = this.filterSignal.asReadonly();
  loading = this.loadingSignal.asReadonly();
  
  // Computed signals
  filteredTodos = computed(() => {
    const todos = this.todosSignal();
    const filter = this.filterSignal();
    
    switch(filter) {
      case 'completed':
        return todos.filter(t => t.completado);
      case 'pending':
        return todos.filter(t => !t.completado);
      default:
        return todos;
    }
  });
  
  setTodos(todos: Todo[]) {
    this.todosSignal.set(todos);
  }
  
  setStats(stats: TodoStats) {
    this.statsSignal.set(stats);
  }
  
  setFilter(filter: 'all' | 'completed' | 'pending') {
    this.filterSignal.set(filter);
  }
  
  setLoading(loading: boolean) {
    this.loadingSignal.set(loading);
  }
  
  addTodo(todo: Todo) {
    this.todosSignal.update(todos => [...todos, todo]);
    this.updateStatsLocally();
  }
  
  updateTodo(updatedTodo: Todo) {
    this.todosSignal.update(todos =>
      todos.map(t => t.id === updatedTodo.id ? updatedTodo : t)
    );
    this.updateStatsLocally();
  }
  
  removeTodo(id: number) {
    this.todosSignal.update(todos => todos.filter(t => t.id !== id));
    this.updateStatsLocally();
  }
  
  clear() {
    this.todosSignal.set([]);
    this.statsSignal.set({ total: 0, completed: 0, pending: 0 });
    this.filterSignal.set('all');
  }
  
  private updateStatsLocally() {
    const todos = this.todosSignal();
    this.statsSignal.set({
      total: todos.length,
      completed: todos.filter(t => t.completado).length,
      pending: todos.filter(t => !t.completado).length
    });
  }
}
