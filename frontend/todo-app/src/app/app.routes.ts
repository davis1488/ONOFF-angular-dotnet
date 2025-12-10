import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { TodoPageComponent } from './todo/todo-page/todo-page.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { 
    path: 'todo', 
    component: TodoPageComponent,
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: '' }
];
