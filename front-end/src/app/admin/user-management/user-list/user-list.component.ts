import { iUser } from './../../../models/i-user';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { UserEditComponent } from '../user-edit/user-edit.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss',
})
export class UserListComponent implements OnInit {
  users: iUser[] = [];
  errorMessage: string | null = '';

  constructor(private userSvc: UserService, private modalSvc: NgbModal) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
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

  openModal(user?: iUser) {
    const modalRef = this.modalSvc.open(UserEditComponent);
    modalRef.componentInstance.user = user;

    modalRef.componentInstance.userUpdated.subscribe(() => {
      this.loadUsers();
    });
  }
}
