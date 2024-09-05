import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { iUser } from '../../../models/i-user';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss',
})
export class UserListComponent {
  users: iUser[] = [];

  constructor(private userSvc: UserService) {}

  ngOnInit(): void {
    this.userSvc.getAll().subscribe((users) => {
      this.users = users;
      console.log(this.users);
    });
  }
}
