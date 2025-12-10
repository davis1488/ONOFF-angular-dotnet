import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TodoService, Todo } from '../../services/todo.service';
import { TodoStore } from '../../services/todo.store';
import { AuthService } from '../../services/auth.service';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  standalone: true,
  selector: 'app-todo-page',
  imports: [
    CommonModule,
    FormsModule,
    MatToolbarModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatTooltipModule,
    MatBadgeModule,
    DashboardComponent
  ],
  templateUrl: './todo-page.component.html',
  styleUrl: './todo-page.component.scss'
})
export class TodoPageComponent implements OnInit {
  private todoService = inject(TodoService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);
  
  store = inject(TodoStore);
  
  nuevoTitulo = '';

  ngOnInit() {
    this.loadTodos();
    this.loadStats();
  }

  loadTodos() {
    this.store.setLoading(true);
    this.todoService.getTodos(this.store.filter()).subscribe({
      next: (todos) => {
        this.store.setTodos(todos);
        this.store.setLoading(false);
      },
      error: () => {
        this.store.setLoading(false);
      }
    });
  }

  loadStats() {
    this.todoService.getStats().subscribe({
      next: (stats) => this.store.setStats(stats)
    });
  }

  agregar() {
    if (!this.nuevoTitulo.trim() || this.nuevoTitulo.length < 3) {
      this.snackBar.open('El título debe tener al menos 3 caracteres', 'Cerrar', {
        duration: 3000,
        panelClass: ['warning-snackbar']
      });
      return;
    }

    this.store.setLoading(true);
    this.todoService.addTodo(this.nuevoTitulo).subscribe({
      next: (todo) => {
        this.store.addTodo(todo);
        this.nuevoTitulo = '';
        this.store.setLoading(false);
        this.snackBar.open('Tarea creada exitosamente', 'Cerrar', {
          duration: 2000,
          panelClass: ['success-snackbar']
        });
      },
      error: () => {
        this.store.setLoading(false);
      }
    });
  }

  toggle(todo: Todo) {
    const updatedTodo = { ...todo, completado: !todo.completado };
    
    this.todoService.updateTodo(updatedTodo).subscribe({
      next: (result) => {
        this.store.updateTodo(result);
        const message = result.completado ? '¡Tarea completada!' : 'Tarea marcada como pendiente';
        this.snackBar.open(message, 'Cerrar', {
          duration: 2000,
          panelClass: ['success-snackbar']
        });
      }
    });
  }

  eliminar(id: number) {
    this.todoService.deleteTodo(id).subscribe({
      next: () => {
        this.store.removeTodo(id);
        this.snackBar.open('Tarea eliminada', 'Cerrar', {
          duration: 2000,
          panelClass: ['success-snackbar']
        });
      }
    });
  }

  setFilter(filter: 'all' | 'completed' | 'pending') {
    this.store.setFilter(filter);
    this.loadTodos();
  }

  logout() {
    this.authService.logout();
    this.store.clear();
    this.router.navigate(['/']);
    this.snackBar.open('Sesión cerrada', 'Cerrar', {
      duration: 2000
    });
  }

  get currentUser() {
    return this.authService.getCurrentUser();
  }

  trackByFn(index: number, item: Todo) {
    return item.id;
  }
}
