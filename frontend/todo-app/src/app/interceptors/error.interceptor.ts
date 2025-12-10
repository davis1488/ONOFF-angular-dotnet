import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);
  
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'Ocurrió un error';
      
      if (error.status === 401) {
        errorMessage = 'Sesión expirada. Por favor, inicia sesión nuevamente.';
        localStorage.clear();
        router.navigate(['/']);
      } else if (error.status === 403) {
        errorMessage = 'Acceso denegado';
      } else if (error.status === 404) {
        errorMessage = 'Recurso no encontrado';
      } else if (error.status >= 500) {
        errorMessage = 'Error del servidor';
      } else if (error.error?.message) {
        errorMessage = error.error.message;
      }
      
      snackBar.open(errorMessage, 'Cerrar', {
        duration: 4000,
        horizontalPosition: 'end',
        verticalPosition: 'top',
        panelClass: ['error-snackbar']
      });
      
      return throwError(() => error);
    })
  );
};
