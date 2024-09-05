import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { iUser } from '../models/i-user';
import { CrudService } from './crud.service';

@Injectable({
  providedIn: 'root',
})
export class UserService extends CrudService<iUser> {
  private userUrl: string = 'User';

  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint(this.userUrl);
  }
}
