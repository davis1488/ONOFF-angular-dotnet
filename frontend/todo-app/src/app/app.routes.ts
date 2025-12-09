import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { TodoPageComponent } from './todo/todo-page/todo-page.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'todo', component: TodoPageComponent }
];
