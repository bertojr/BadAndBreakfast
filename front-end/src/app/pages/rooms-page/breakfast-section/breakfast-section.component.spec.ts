import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BreakfastSectionComponent } from './breakfast-section.component';

describe('BreakfastSectionComponent', () => {
  let component: BreakfastSectionComponent;
  let fixture: ComponentFixture<BreakfastSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BreakfastSectionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BreakfastSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
