import { Component } from '@angular/core';
import { iUser } from '../../models/i-user';
import { AuthService } from '../auth.service';
import { catchError, of } from 'rxjs';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  newUser: Partial<iUser> = {};
  errorMessage: string | null = null;

  constructor(private authSvc: AuthService) {}

  register(form: NgForm) {
    if (form.invalid) {
      this.errorMessage = 'Per favore correggi gli errori nel modulo';
      return;
    }

    this.authSvc.register(this.newUser).subscribe({
      next: () => {
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la registrazione';
      },
    });
  }
}
