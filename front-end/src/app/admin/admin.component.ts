import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserEditComponent } from './user-management/user-edit/user-edit.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
})
export class AdminComponent {
  constructor(private modalSvc: NgbModal) {}

  openModal(item: any) {
    const modalRef = this.modalSvc.open(UserEditComponent);
  }
}
