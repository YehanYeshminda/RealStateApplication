import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyregisterComponent } from './propertyregister.component';

describe('PropertyregisterComponent', () => {
  let component: PropertyregisterComponent;
  let fixture: ComponentFixture<PropertyregisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PropertyregisterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PropertyregisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
