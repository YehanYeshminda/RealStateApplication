import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { CommonHttpService } from '../services/common-http.service';
import { getAuthDetails } from '../shared/method';
import { ComboInfo } from '../shared/models/models';
import { infoNotification, successNotification, errorNotification } from '../shared/notifications/notification';
import { PropertyRegisterView } from './Model/propertyregister';
import { PropertyregisterService } from './Service/propertyregister.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-propertyregister',
  templateUrl: './propertyregister.component.html',
  styleUrls: ['./propertyregister.component.scss']
})

export class PropertyregisterComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  dynamicform: FormGroup = new FormGroup({});
  propreginfo$!: PropertyRegisterView;
  propsubcat$: Observable<ComboInfo[]> = of([]);
  propregtype$: Observable<ComboInfo[]> = of([]);
  propcattype$: Observable<ComboInfo[]> = of([]);
  citytype$: Observable<ComboInfo[]> = of([]);
  vendertype$: Observable<ComboInfo[]> = of([]);
  usertype$: Observable<ComboInfo[]> = of([]);
  isEditMode = false;
  itemAdded = false;
  submitted = false;
  submitted1 = false;
  submitted2 = false;
  submitted3 = false;
  userImage = '';
  baseUrl = environment.apiUrl;
  @ViewChild('userLogo') userLogoInput!: ElementRef;
  @ViewChild('userLogoOther') userLogoInputOther!: ElementRef;
  hostUrl: string = '';
  updatedImage = ''
  selectedFileName: string = '';
  imageUrl: string | ArrayBuffer | null | undefined = null;
  otherImages: Array<{ url: string }> = []; 
  
  get form1() { return this.form.controls; }

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private propertyregistrationservices: PropertyregisterService,
    private router: Router, 
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.loadPropsubcat();
    this.loadpropreg();
    this.loadPropcat();
    this.loadcity();
    this.loadSupplier();
    this.initializeForm();
    this.getLastNo();
    this.initializedynamicForm();
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.form.get('mainImg')?.setValue(file);
    this.previewImage(file);

    this.selectedFileName = file ? file.name : '';
  }


  onImageChange(image: string) {
    this.baseUrl = this.baseUrl.replace('/api', '');
    const imagePath = 'upload/property/main/' + image;
    this.imageUrl = this.baseUrl + imagePath;
    this.updatedImage = image;

    this.selectedFileName = image ? image : '';
  }

  previewImage(file: File) {
    const reader = new FileReader();
    reader.onload = () => {
      this.imageUrl = reader.result?.toString();
    };
    reader.readAsDataURL(file);
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('PR', 'tblpropertyregister', 'id').subscribe({
        next: response => {
          this.form.patchValue({
            id: response.lastValue
          })
        }
      });
    }
  }

  loadPropsubcat() {
    const query =
      "SELECT ID as _Id, propertySubCat as _Value FROM tblpropertySubCategory order by ID asc";
    this.propsubcat$ = this.commonHttpService.getComboBoxData(query);
  }

  loadPropcat() {
    const query =
      "SELECT ID as _Id, propertyCat as _Value FROM tblpropertyCategory order by ID asc";
    this.propcattype$ = this.commonHttpService.getComboBoxData(query);
  }

  loadpropreg() {
    const query =
      "SELECT ID as _Id, propertytype as _Value FROM tblpropertytype order by ID asc";
    this.propregtype$ = this.commonHttpService.getComboBoxData(query);
  }

  loadcity() {
    const query =
      "SELECT TypeID as _Id, TypeName as _Value FROM tblCitytype order by TypeID asc";
    this.citytype$ = this.commonHttpService.getComboBoxData(query);
  }


  loadSupplier() {
    const query =
      "SELECT SupplierID as _Id, SupplierName as _Value FROM tblSupplier order by SupplierName asc";
    this.vendertype$ = this.commonHttpService.getComboBoxData(query);
  }

  updateCheckboxValue(controlName: string, checked: boolean) {
    const value = checked ? 1 : 0;
    this.form.get(controlName)?.setValue(value);
  }

  uploadtype(content: TemplateRef<NgbModal>): void {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }

  initializedynamicForm() {
    this.dynamicform = this.fb.group({
      catergoryName: ["", [Validators.required]],
      remark: ["", [Validators.required]],
      status: [0, [Validators.required]],
    })
  }

  addNewDesignation(){}

  async addEditItems() {
    if (this.form.controls['mainImg'].value == '' || this.form.controls['Otherimg'].value == '') {
      infoNotification("Please select main image and other images to proceed")
      return;
    }

    this.itemAdded = true;
    try {
      if (this.propreginfo$.Id) {
        await this.editPropertyInfo();
      } else {
        await this.addPropertyInfo();
      }

      this.itemAdded = false;
    } catch (error) {
      console.error('Error processing item:', error);
      this.itemAdded = false;
    }

  }

  initializeForm() {
    this.form = this.fb.group({
      id: [''],
      type: ['', [Validators.required]],
      propertname: ['', [Validators.required]],
      category: ['', [Validators.required]],
      subcategory: ['', [Validators.required]],
      city: ['', [Validators.required]],
      nationality: ['', [Validators.required]],
      address: ['', [Validators.required]],
      geolocation: ['', [Validators.required]],
      vender: ['', [Validators.required]],
      costanually: ['', [Validators.required]],
      othercost: ['', [Validators.required]],
      rulesregulations: ['', [Validators.required]],
      status: ['', Validators.required],
      sellingprice: ['', [Validators.required]],
      minsellingprice: ['', [Validators.required]],
      anualcostforbuyer: ['', [Validators.required]],
      deposit: ['', [Validators.required]],
      contacttype: ['', [Validators.required]],
      socialmedia: [''],
      mainImg: ['', []],
      Otherimg: ['', []],
      dateofpurchorrent: [getCurrentDate(), [Validators.required]],
      renewdate: [getCurrentDate(), [Validators.required]],
      venderpaymentdate: [getCurrentDate(), [Validators.required]],
      paymentscheduleno: ['', [Validators.required]],
    });

    this.propreginfo$ = history.state;

    if (this.propreginfo$.Id) {
      this.isEditMode = true;
      this.form.patchValue({
        ...this.propreginfo$,
        id: this.propreginfo$.Id,
        dateofpurchorrent: new Date(this.propreginfo$.Dateofpurchorrent).toISOString().split('T')[0],
        renewdate: new Date(this.propreginfo$.Renewdate).toISOString().split('T')[0],
        venderpaymentdate: new Date(this.propreginfo$.Venderpaymentdate).toISOString().split('T')[0],
        propertname: this.propreginfo$.PropertyName,
        nationality: this.propreginfo$.Nationality,
        address: this.propreginfo$.Address,
        geolocation: this.propreginfo$.geolocation,
        costanually: this.propreginfo$.CostAnually,
        othercost: this.propreginfo$.Othercost,
        rulesregulations: this.propreginfo$.Rulesregulations,
        contacttype: this.propreginfo$.Contacttype,
        sellingprice: this.propreginfo$.Sellingprice,
        minsellingprice: this.propreginfo$.Minsellingprice,
        anualcostforbuyer: this.propreginfo$.Anualcostforbuyer,
        deposit: this.propreginfo$.Deposit,
        status: this.propreginfo$.Status,
        socialmedia: this.propreginfo$.Socialmedia,
        paymentscheduleno: this.propreginfo$.Paymentscheduleno,
      });

      this.propregtype$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.propreginfo$.PropertyType);
          this.form.patchValue({
            type: values[existingValue].value,
          })
        }
      })

      this.propcattype$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.propreginfo$.PropertyCatergory);
          this.form.patchValue({
            category: values[existingValue].value,
          })
        }
      })

      this.propsubcat$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.propreginfo$.PropertySubCatergory);
          this.form.patchValue({
            subcategory: values[existingValue].value,
          })
        }
      })

      this.citytype$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.propreginfo$.TypeName);
          this.form.patchValue({
            city: values[existingValue].value,
          })
        }
      })

      this.vendertype$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.propreginfo$.SupplierName);
          this.form.patchValue({
            vender: values[existingValue].value,
          })
        }
      })
    } else {
      this.isEditMode = false;
    }
  }

  // CONVERTS STRING TO BLOB
  dataURItoBlob(dataURI: string): Blob {
    const splitDataURI = dataURI.split(',');
    const dataType = splitDataURI[0];
    const base64Data = splitDataURI[1];

    const contentTypeMatches = dataType.match(/^data:(.*?);/);
    const contentType = contentTypeMatches ? contentTypeMatches[1] : '';

    const binaryData = atob(base64Data);
    const arrayBuffer = new ArrayBuffer(binaryData.length);
    const uint8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < binaryData.length; i++) {
      uint8Array[i] = binaryData.charCodeAt(i);
    }

    return new Blob([uint8Array], { type: contentType });
  }


  addPropertyInfo(): void {
    const formData = new FormData();
    const auth = getAuthDetails();
    formData.append('id', this.form.value.id);
    formData.append('type', this.form.value.type);
    formData.append('propertname', this.form.value.propertname);
    formData.append('category', this.form.value.category);
    formData.append('subcategory', this.form.value.subcategory);
    formData.append('city', this.form.value.city);
    formData.append('nationality', this.form.value.nationality);
    formData.append('address', this.form.value.address);
    formData.append('geolocation', this.form.value.geolocation);
    formData.append('vender', this.form.value.vender);
    formData.append('costanually', this.form.value.costanually);
    formData.append('othercost', this.form.value.othercost);
    formData.append('rulesregulations', this.form.value.rulesregulations);
    formData.append('status', this.form.value.status);
    formData.append('sellingprice', this.form.value.sellingprice);
    formData.append('minsellingprice', this.form.value.minsellingprice);
    formData.append('anualcostforbuyer', this.form.value.anualcostforbuyer);
    formData.append('deposit', this.form.value.deposit);
    formData.append('contacttype', this.form.value.contacttype);
    formData.append('Socialmedia', "1");
    formData.append('dateofpurchorrent', this.form.value.dateofpurchorrent);
    formData.append('renewdate', this.form.value.renewdate);
    formData.append('venderpaymentdate', this.form.value.venderpaymentdate);
    formData.append('paymentscheduleno', this.form.value.paymentscheduleno);
    formData.append('Hash', auth.hash);
    formData.append('MainImg', this.form.get('mainImg')?.value);

    for (let i = 0; i < this.otherImages.length; i++) {
      const image = this.otherImages[i];
      const blob = this.dataURItoBlob(image.url);
      formData.append('OtherImages' + i, blob, 'image' + i + '.png');
    }


    this.propertyregistrationservices.addpropertyregister(formData).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/propertyregistrationlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


  editPropertyInfo(): void {
    const formData = new FormData();
    const auth = getAuthDetails();
    formData.append('id', this.form.value.id);
    formData.append('type', this.form.value.type);
    formData.append('propertname', this.form.value.propertname);
    formData.append('category', this.form.value.category);
    formData.append('subcategory', this.form.value.subcategory);
    formData.append('city', this.form.value.city);
    formData.append('nationality', this.form.value.nationality);
    formData.append('address', this.form.value.address);
    formData.append('geolocation', this.form.value.geolocation);
    formData.append('vender', this.form.value.vender);
    formData.append('costanually', this.form.value.costanually);
    formData.append('othercost', this.form.value.othercost);
    formData.append('rulesregulations', this.form.value.rulesregulations);
    formData.append('status', this.form.value.status);
    formData.append('sellingprice', this.form.value.sellingprice);
    formData.append('minsellingprice', this.form.value.minsellingprice);
    formData.append('anualcostforbuyer', this.form.value.anualcostforbuyer);
    formData.append('deposit', this.form.value.deposit);
    formData.append('contacttype', this.form.value.contacttype);
    formData.append('Socialmedia', "1");
    formData.append('dateofpurchorrent', this.form.value.dateofpurchorrent);
    formData.append('renewdate', this.form.value.renewdate);
    formData.append('venderpaymentdate', this.form.value.venderpaymentdate);
    formData.append('paymentscheduleno', this.form.value.paymentscheduleno);
    formData.append('Hash', auth.hash);
    formData.append('MainImg', this.form.get('mainImg')?.value);

    for (let i = 0; i < this.otherImages.length; i++) {
      const image = this.otherImages[i];
      const blob = this.dataURItoBlob(image.url);
      formData.append('OtherImages' + i, blob, 'image' + i + '.png');
    }


    this.propertyregistrationservices.updatepropertyregister(formData).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/propertyregistrationlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  onFileChangeMulti(event: any) {
    if (this.otherImages.length < 20) {
      if (event.target.files && event.target.files.length > 0) {
        const files = event.target.files;
        for (let i = 0; i < files.length; i++) {
          const file = files[i];
          const reader = new FileReader();
          reader.onload = (e: any) => {
            this.otherImages.push({ url: e.target.result });
          };
          reader.readAsDataURL(file);
        }
      }
    } else {
      errorNotification("Maximum of 20 images are allowed!")
    }
  }

}






function getCurrentDate(): any {
  throw new Error('Function not implemented.');
}

