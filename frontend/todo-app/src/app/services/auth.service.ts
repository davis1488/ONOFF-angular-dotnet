import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

interface LoginResponse {
  token: string;
  user: {
    id: number;
    email: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'jwt_token';
  private readonly TOKEN_EXPIRY_KEY = 'jwt_token_expiry';
  private readonly USER_KEY = 'current_user';

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<LoginResponse>('/api/Auth/login', { email, password })
      .pipe(
        tap(res => {
          localStorage.setItem(this.TOKEN_KEY, res.token);
          localStorage.setItem(this.USER_KEY, JSON.stringify(res.user));
          
          // Decodificar JWT para obtener exp
          const payload = this.decodeToken(res.token);
          if (payload.exp) {
            localStorage.setItem(this.TOKEN_EXPIRY_KEY, payload.exp.toString());
          }
        })
      );
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.TOKEN_EXPIRY_KEY);
    localStorage.removeItem(this.USER_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getCurrentUser() {
    const userStr = localStorage.getItem(this.USER_KEY);
    return userStr ? JSON.parse(userStr) : null;
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    const expiry = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
    
    if (!token || !expiry) return false;
    
    // Verificar si el token expiró
    const now = Math.floor(Date.now() / 1000);
    if (parseInt(expiry) < now) {
      this.logout();
      return false;
    }
    
    return true;
  }

  private decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      return JSON.parse(window.atob(base64));
    } catch (e) {
      return {};
    }
  }
}
