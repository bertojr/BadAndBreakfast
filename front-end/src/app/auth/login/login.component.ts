import { Component } from '@angular/core';
import { iAuthData } from '../../models/i-auth-data';
import { AuthService } from '../auth.service';
import { catchError, of } from 'rxjs';
import { NgForm } from '@angular/forms';

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

  errorMessage: string | null = null;

  constructor(private authSvc: AuthService) {}

  login(form: NgForm) {
    this.authSvc
      .login(this.authData)
      .pipe(
        catchError((error) => {
          alert(error.error.message);
          return of(null);
        })
      )
      .subscribe((response) => {
        if (response) {
          this.errorMessage = null;
        }
      });
  }
}
