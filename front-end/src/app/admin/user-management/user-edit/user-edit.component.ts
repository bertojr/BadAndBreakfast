import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { iUser } from '../../../models/i-user';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss',
})
export class UserEditComponent {
  userForm!: FormGroup;
  user!: iUser;
  errorMessage: string | any = '';

  constructor(
    private route: ActivatedRoute,
    private userSvc: UserService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.route.params.subscribe((params: any) => {
      const userId = params.id;
      if (userId) {
        // se esiste un id recupera l'utente dal servizio
        this.userSvc.getById(userId).subscribe({
          next: (user) => {
            this.user = user;
            console.log(this.user);
            this.populateForm(user);
            this.errorMessage = null;
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la ricerca';
          },
        });
      }
    });
  }

  private initializeForm(): void {
    this.userForm = this.fb.group({
      name: this.fb.control(''),
      email: this.fb.control('', [Validators.required, Validators.email]),
      cell: this.fb.control(''),
      dateOfBirth: this.fb.control(''),
      nationally: this.fb.control(''),
      gender: this.fb.control(''),
      password: this.fb.control(''),
      country: this.fb.control(''),
      address: this.fb.control(''),
      city: this.fb.control(''),
      cap: this.fb.control(''),
    });
  }

  private populateForm(user: iUser): void {
    this.userForm.patchValue({
      name: user.name || '',
      email: user.email || '',
      cell: user.cell || '',
      dateOfBirth: user.dateOfBirth || '',
      nationally: user.nationally || '',
      gender: user.gender || '',
      password: user.password || '',
      country: user.country || '',
      address: user.address || '',
      city: user.city || '',
      cap: user.cap || '',
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      if (this.user) {
        const userUpdate: iUser = this.userForm.value;
        this.userSvc.update(this.user.userID, userUpdate).subscribe({
          next: () => {
            this.errorMessage = null;
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la ricerca';
          },
        });
      } else {
        const newUser: Partial<iUser> = this.userForm.value;
        this.userSvc.create(newUser).subscribe({
          next: () => {
            this.errorMessage = null;
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la ricerca';
          },
        });
      }
    }
  }
}
