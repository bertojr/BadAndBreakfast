import { Injectable } from '@angular/core';
import { CrudService } from './crud.service';
import { iAdditionalService } from '../models/i-additional-service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AdditionalServiceService extends CrudService<iAdditionalService> {
  constructor(http: HttpClient) {
    super(http);
    this.setEndpoint('AdditionalService');
  }
}
