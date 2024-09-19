import { Component, EventEmitter, Input, input, Output } from '@angular/core';
import { RoleService } from '../../../services/role.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { iRole } from '../../../models/i-role';

@Component({
  selector: 'app-role-edit',
  templateUrl: './role-edit.component.html',
  styleUrl: './role-edit.component.scss',
})
export class RoleEditComponent {
  @Input() role!: iRole; // dati dal componente chiamante
  @Output() roleUpdated = new EventEmitter<void>();
  errorMessage: string | any = '';

  constructor(
    private activeModal: NgbActiveModal,
    private roleSvc: RoleService
  ) {}

  onSubmit(form: NgForm): void {
    if (form.valid) {
      if (this.role.roleID) {
        this.roleSvc.update(this.role.roleID, this.role).subscribe({
          next: () => {
            this.errorMessage = null;
            this.roleUpdated.emit();
            this.closeModal();
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la modifica';
          },
        });
      } else {
        const newRole: Partial<iRole> = {
          name: this.role.name,
          description: this.role.description,
        };
        this.roleSvc.create(this.role).subscribe({
          next: () => {
            this.errorMessage = null;
            this.roleUpdated.emit();
            this.closeModal();
          },
          error: (error) => {
            this.errorMessage =
              error.message || 'Si è verificato un errore durante la creazione';
          },
        });
      }
    } else {
      this.errorMessage = 'Form non valido';
    }
  }

  closeModal() {
    this.activeModal.close();
  }
}
