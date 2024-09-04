import { Component } from '@angular/core';
import { iAuthData } from '../../models/i-auth-data';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  authData: iAuthData = {
    email: 'admin@admin.com',
    password: 'password',
  };

  constructor(private authSvc: AuthService) {}

  login() {
    this.authSvc.login(this.authData).subscribe(
      (data) => {
        console.log('Login successful:', data);
      },
      (error) => {
        console.error('Error during login:', error);
      }
    );
  }
}
