import { Component, OnInit } from '@angular/core';
import { iRole } from '../../../models/i-role';
import { RoleService } from '../../../services/role.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RoleEditComponent } from '../role-edit/role-edit.component';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.scss',
})
export class RoleListComponent implements OnInit {
  roles: iRole[] = [];
  errorMessage: string | null = '';

  constructor(private roleSvc: RoleService, private modalSvc: NgbModal) {}

  ngOnInit(): void {
    this.loadRoles();
  }

  private loadRoles(): void {
    this.roleSvc.getAll().subscribe({
      next: (roles) => {
        this.roles = roles;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la ricerca';
      },
    });
  }

  deleteUser(id: number): void {
    this.roleSvc.delete(id).subscribe({
      next: () => {
        this.roles = this.roles.filter((role) => role.roleID !== id);
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la eliminazione';
      },
    });
  }

  openModal(role?: iRole) {
    const modalRef = this.modalSvc.open(RoleEditComponent);
    modalRef.componentInstance.role = role ? role : {};

    modalRef.componentInstance.roleUpdated.subscribe(() => {
      this.loadRoles();
    });
  }
}
