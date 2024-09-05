import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { iUser } from '../../../models/i-user';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss',
})
export class UserEditComponent {
  user!: iUser;
  errorMessage: string | any = '';

  constructor(private route: ActivatedRoute, private userSvc: UserService) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.userSvc.getById(params.id).subscribe({
        next: (user) => {
          this.user = user;
          this.errorMessage = null;
        },
        error: (error) => {
          this.errorMessage =
            error.message || 'Si è verificato un errore durante la ricerca';
        },
      });
    });
  }
}
