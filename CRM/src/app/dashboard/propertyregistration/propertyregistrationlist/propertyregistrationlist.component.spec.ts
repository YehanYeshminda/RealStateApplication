import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyregistrationlistComponent } from './propertyregistrationlist.component';

describe('PropertyregistrationlistComponent', () => {
  let component: PropertyregistrationlistComponent;
  let fixture: ComponentFixture<PropertyregistrationlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PropertyregistrationlistComponent]
    });
    fixture = TestBed.createComponent(PropertyregistrationlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
