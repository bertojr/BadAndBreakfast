import { Component } from '@angular/core';

@Component({
  selector: 'app-breakfast-section',
  templateUrl: './breakfast-section.component.html',
  styleUrl: './breakfast-section.component.scss',
})
export class BreakfastSectionComponent {
  scrollToBookings(): void {
    const bookingElement = document.getElementById('bookings');
    if (bookingElement) {
      bookingElement.scrollIntoView({ behavior: 'smooth' });
    }
  }
}
