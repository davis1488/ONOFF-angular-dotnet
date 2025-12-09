import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Todo {
  id: number;
  titulo: string;
  completado: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  private apiUrl = '/api/Todo';

  constructor(private http: HttpClient) {}

  getTodos(): Observable<Todo[]> {
    return this.http.get<Todo[]>(this.apiUrl);
  }

  addTodo(titulo: string): Observable<Todo> {
    return this.http.post<Todo>(this.apiUrl, { titulo });
  }

  updateTodo(todo: Todo): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${todo.id}`, todo);
  }

  deleteTodo(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
