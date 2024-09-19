import { Component } from '@angular/core';
import { iAdditionalService } from '../../../models/i-additional-service';
import { AdditionalServiceService } from '../../../services/additional-service.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdditionalServiceEditComponent } from '../additional-service-edit/additional-service-edit.component';

@Component({
  selector: 'app-additional-service-list',
  templateUrl: './additional-service-list.component.html',
  styleUrl: './additional-service-list.component.scss',
})
export class AdditionalServiceListComponent {
  services: iAdditionalService[] = [];
  errorMessage: string | null = '';

  constructor(
    private additionaServiceSvc: AdditionalServiceService,
    private modalSvc: NgbModal
  ) {}

  ngOnInit(): void {
    this.loadServices();
  }

  loadServices(): void {
    this.additionaServiceSvc.getAll().subscribe({
      next: (services) => {
        this.services = services;
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la ricerca';
      },
    });
  }

  deleteService(id: number): void {
    this.additionaServiceSvc.delete(id).subscribe({
      next: () => {
        this.services = this.services.filter(
          (service) => service.serviceID !== id
        );
        this.errorMessage = null;
      },
      error: (error) => {
        this.errorMessage =
          error.message || 'Si è verificato un errore durante la eliminazione';
      },
    });
  }

  openModal(service?: iAdditionalService): void {
    const modalRef = this.modalSvc.open(AdditionalServiceEditComponent);
    modalRef.componentInstance.service = service ? service : {};

    modalRef.componentInstance.serviceUpdated.subscribe(() => {
      this.loadServices();
    });
  }
}
