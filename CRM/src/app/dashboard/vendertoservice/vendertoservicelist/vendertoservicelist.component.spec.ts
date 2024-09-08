import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendertoservicelistComponent } from './vendertoservicelist.component';

describe('VendertoservicelistComponent', () => {
  let component: VendertoservicelistComponent;
  let fixture: ComponentFixture<VendertoservicelistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VendertoservicelistComponent]
    });
    fixture = TestBed.createComponent(VendertoservicelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
