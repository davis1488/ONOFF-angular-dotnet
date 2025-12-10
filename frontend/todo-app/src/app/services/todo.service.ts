import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Todo {
  id: number;
  titulo: string;
  completado: boolean;
  userId: number;
}

export interface TodoStats {
  total: number;
  completed: number;
  pending: number;
}

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private apiUrl = '/api/Todo';

  constructor(private http: HttpClient) {}

  getTodos(filter: 'all' | 'completed' | 'pending' = 'all'): Observable<Todo[]> {
    return this.http.get<Todo[]>(`${this.apiUrl}?filter=${filter}`);
  }

  getStats(): Observable<TodoStats> {
    return this.http.get<TodoStats>(`${this.apiUrl}/stats`);
  }

  addTodo(titulo: string): Observable<Todo> {
    return this.http.post<Todo>(this.apiUrl, { titulo });
  }

  updateTodo(todo: Todo): Observable<Todo> {
    return this.http.put<Todo>(`${this.apiUrl}/${todo.id}`, {
      titulo: todo.titulo,
      completado: todo.completado
    });
  }

  deleteTodo(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
