import { Component } from '@angular/core';
import { iAuthData } from '../../models/i-auth-data';
import { AuthService } from '../auth.service';
import { catchError, of } from 'rxjs';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

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

  constructor(
    private authSvc: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  login(form: NgForm) {
    this.authSvc.login(this.authData).subscribe({
      next: () => {
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante il login';
      },
    });

    // Ottieni il parametro returnUrl se presente
    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.router.navigate([returnUrl]); // Reindirizza alla pagina desiderata o home
  }
}
