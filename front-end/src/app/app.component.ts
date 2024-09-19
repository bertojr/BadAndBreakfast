import { Component } from '@angular/core';
import { iUser } from './models/i-user';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'front-end';

  user: any | null = null;

  constructor(private authSvc: AuthService) {}

  ngOnInit(): void {
    this.authSvc.user$.subscribe((user) => {
      this.user = user;
    });
  }
}
