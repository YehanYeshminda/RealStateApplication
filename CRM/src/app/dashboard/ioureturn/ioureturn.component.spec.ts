import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IoureturnComponent } from './ioureturn.component';

describe('IoureturnComponent', () => {
  let component: IoureturnComponent;
  let fixture: ComponentFixture<IoureturnComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IoureturnComponent]
    });
    fixture = TestBed.createComponent(IoureturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
