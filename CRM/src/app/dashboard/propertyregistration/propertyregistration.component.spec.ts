import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyregistrationComponent } from './propertyregistration.component';

describe('PropertyregistrationComponent', () => {
  let component: PropertyregistrationComponent;
  let fixture: ComponentFixture<PropertyregistrationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PropertyregistrationComponent]
    });
    fixture = TestBed.createComponent(PropertyregistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
