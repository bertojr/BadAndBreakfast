import { Component } from '@angular/core';
import { iUser } from '../../models/i-user';
import { AuthService } from '../../auth/auth.service';
import { iRole } from '../../models/i-role';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss',
})
export class NavBarComponent {
  user!: iUser | null;

  constructor(private authSvc: AuthService) {}

  ngOnInit(): void {
    this.authSvc.user$.subscribe((user) => {
      this.user = user;
    });

    console.log(this.user);
  }

  logout(): void {
    this.authSvc.logout();
  }

  isAdmin(): boolean | undefined {
    return this.user?.roles?.some((role: iRole) => role.name === 'Admin');
  }
}
