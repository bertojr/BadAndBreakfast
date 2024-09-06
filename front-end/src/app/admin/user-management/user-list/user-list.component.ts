import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { iUser } from '../../../models/i-user';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss',
})
export class UserListComponent {
  users: iUser[] = [];
  errorMessage: string | null = '';

  constructor(private userSvc: UserService) {}

  ngOnInit(): void {
    this.userSvc.getAll().subscribe({
      next: (users) => {
        this.users = users;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la ricerca';
      },
    });
  }

  deleteUser(id: number): void {
    this.userSvc.delete(id).subscribe({
      next: () => {
        this.users = this.users.filter((user) => user.userID !== id);
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la eliminazione';
      },
    });
  }
}
