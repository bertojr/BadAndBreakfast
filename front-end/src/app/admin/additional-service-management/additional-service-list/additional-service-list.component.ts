import { Component } from '@angular/core';
import { iAdditionalService } from '../../../models/i-additional-service';
import { AdditionalServiceService } from '../../../services/additional-service.service';

@Component({
  selector: 'app-additional-service-list',
  templateUrl: './additional-service-list.component.html',
  styleUrl: './additional-service-list.component.scss',
})
export class AdditionalServiceListComponent {
  services: iAdditionalService[] = [];
  errorMessage: string | null = '';

  constructor(private additionaServiceSvc: AdditionalServiceService) {}

  ngOnInit(): void {
    this.getAll();
  }

  getAll(): void {
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
}
