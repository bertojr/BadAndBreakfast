import { Component, EventEmitter, Input, input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { iUser } from '../../../models/i-user';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../auth/auth.service';
import { RoleService } from '../../../services/role.service';
import { iRole } from '../../../models/i-role';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss',
})
export class UserEditComponent {
  userForm!: FormGroup;
  @Input() user!: iUser; // dati dal componente chiamante
  @Output() userUpdated = new EventEmitter<void>();
  roles: iRole[] = [];
  errorMessage: string | any = '';
  selectedRoleId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private userSvc: UserService,
    private authSvc: AuthService,
    private fb: FormBuilder,
    private roleSvc: RoleService,
    private activeModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.populateForm(this.user);
    /*
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
    });*/

    // popolamento select dei ruoli
    this.roleSvc.getAll().subscribe({
      next: (roles) => {
        this.roles = roles;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message ||
          'Si è verificato un errore durante la ricerca dei ruoli';
      },
    });
  }

  private initializeForm(): void {
    this.userForm = this.fb.group({
      name: this.fb.control(null),
      email: this.fb.control(null, [Validators.required, Validators.email]),
      cell: this.fb.control(null),
      dateOfBirth: this.fb.control(null),
      nationally: this.fb.control(null),
      gender: this.fb.control(''),
      password: this.fb.control(null, Validators.required),
      passwordHash: this.fb.control(null),
      passwordSalt: this.fb.control(null),
      country: this.fb.control(null),
      address: this.fb.control(null),
      city: this.fb.control(null),
      cap: this.fb.control(null),
    });
  }

  private populateForm(user: iUser): void {
    if (user) {
      const formattedDateOfBirth = user.dateOfBirth
        ? user.dateOfBirth.split('T')[0]
        : null;
      this.userForm.patchValue({
        name: user.name || null,
        email: user.email || null,
        cell: user.cell || null,
        dateOfBirth: formattedDateOfBirth || null,
        nationally: user.nationally || null,
        gender: user.gender || '',
        password: 'non serve a niente',
        passwordHash: user.passwordHash || null,
        passwordSalt: user.passwordSalt || null,
        country: user.country || null,
        address: user.address || null,
        city: user.city || null,
        cap: user.cap || null,
      });
    }
  }

  addRole(): void {
    if (this.selectedRoleId !== null) {
      this.userSvc
        .addRoleToUser(this.user.userID, this.selectedRoleId)
        .subscribe({
          next: (role) => {
            this.user.roles?.push(role);
            this.errorMessage = null;
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la ricerca';
          },
        });
    }
  }

  removeRoleFromUser(userId: number, roleId: number): void {
    this.userSvc.removeRoleFromUser(userId, roleId).subscribe({
      next: () => {
        this.user.roles = this.user.roles?.filter(
          (role) => role.roleID !== roleId
        );
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la eliminazione';
      },
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      if (this.user) {
        const userUpdate: iUser = this.userForm.value;
        console.log(userUpdate);
        this.userSvc.update(this.user.userID, userUpdate).subscribe({
          next: () => {
            this.errorMessage = null;
            this.userUpdated.emit();
            this.closeModal();
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la ricerca';
          },
        });
      } else {
        const newUser: Partial<iUser> = this.userForm.value;
        this.authSvc.register(newUser).subscribe({
          next: () => {
            this.errorMessage = null;
            this.userUpdated.emit();
            this.closeModal();
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la ricerca';
          },
        });
      }
    }
  }

  closeModal() {
    this.activeModal.close();
  }
}
