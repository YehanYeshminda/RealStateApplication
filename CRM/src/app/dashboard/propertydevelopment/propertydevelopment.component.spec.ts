import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertydevelopmentComponent } from './propertydevelopment.component';

describe('PropertydevelopmentComponent', () => {
  let component: PropertydevelopmentComponent;
  let fixture: ComponentFixture<PropertydevelopmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PropertydevelopmentComponent]
    });
    fixture = TestBed.createComponent(PropertydevelopmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
