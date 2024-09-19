import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CrudService } from './crud.service';
import { iRole } from '../models/i-role';

@Injectable({
  providedIn: 'root',
})
export class RoleService extends CrudService<iRole> {
  private urlRole: string = 'Role';

  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint(this.urlRole);
  }
}
