import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BusinessDataService {
  
  isLogging: boolean = false;
  isChecking:boolean=false;
  hashmap:any={};
  public pieDialogRef:any;
  pieLabels:any=[];
  piedata:any=[];
  chartType:any;
  expensesLogged :any=0;
  latestLoginDate:any='';
  firstLoginDate:any=''
  keywords:any;
  data:any;
  apiUrl = environment.apiUrl;
  userId:any;
  appVersion:any;
  private comingSrc:any='Direct';
  constructor(private route: Router, public http: HttpClient) {
  }

  setComingSrc(val:any){
    this.comingSrc=val;
  }

  getComingSrc(){
    return this.comingSrc;
  }

  getUserIdFromSS(){
    return sessionStorage.getItem('Id')?.split(' ')[1];
  }

  onHome(){
    this.route.navigate(['home']);
  }
  onNavigate(url:any){
    this.route.navigate([url]);
  }

  onGetAllExpense(id:any) {
    this.userId=id;
    return this.http.get(this.apiUrl + 'Expenses/GET_ALL_EXPENSE/'+id);
  }

  onCreateExpense(values: any,date:any) {
    let id=sessionStorage.getItem('Id')?.split(' ')[1];
    let body={
      Name: values.name,
      Amount: values.amount,
      ExpenseDate: (date[0]+' '+date[1]+' '+date[2]+' '+date[3]),
      ExpenseCategory: values.expense_category,
      Payment: values.payment,
      Comment: values.comment,
      Creater: id,
    }
    return this.http.post(this.apiUrl + 'Expenses/CREATE_EXPENSE', body);
  }


  onImportExpense(values:any){
    let id=this.getUserIdFromSS();
    let date=values.expense_date.split('/');
    date=(new Date(date[2],date[1]-1,date[0])).toString();
    date=date.split(' ');
    let body={
      Name: values.expense_name,
      Amount: values.amount,
      ExpenseDate: (date[0]+' '+date[1]+' '+date[2]+' '+date[3]),
      ExpenseCategory: values.expense_category,
      Payment: values.payment_type,
      Comment: values.comment,
      Creater: id,
    }
    return this.http.post(this.apiUrl+'Expenses/CREATE_EXPENSE',body);
  }


  onCreateCategory(body:any){
    return this.http.post(this.apiUrl+'SAVE_CATEGORY/'+this.userId,body);
  }

  onEditCategory(body:any){
    return this.http.post(this.apiUrl+'EDIT_CATEGORY/'+this.userId,body);
  }
  
  onDeleteExpense(id:string){
    return this.http.delete(this.apiUrl+'DELETE_EXPENSE/'+this.userId+'/'+id);
  }

  onGetSingleExpense(id:string){
    return this.http.get(this.apiUrl+'GET_SINGLE_EXPENSE/'+this.userId+'/'+id);
  }

  onUpdateExpense(id:string,values:any){
    let str=values.expense_date.toString();
    let date=str.split(' ');
    let body={
      Name: values.name,
      Amount: values.amount,
      ExpenseDate: (date[0]+' '+date[1]+' '+date[2]+' '+date[3]),
      ExpenseCategory: values.expense_category,
      Payment: values.payment,
      Comment: values.comment,
      Creater: this.userId,
    }
    return this.http.patch(this.apiUrl+'UPDATE_EXPENSE/'+this.userId+'/'+id,body);
  }

  onGetAllCategory(){
    this.userId=sessionStorage.getItem('Id')?.split(' ')[1];
    return this.http.get(this.apiUrl+'GET_CATEGORY/'+this.userId);
  }
  
  onGithub(){
    const link=document.createElement('a');
    link.target="_blank";
    link.href="https://github.com/vamsi-pithani";
    link.click();
  }
  onLinkedin(){
    const link=document.createElement('a');
    link.target="_blank";
    link.href="https://www.linkedin.com/in/vamsi-pithani-74708b34a/";
    link.click();
  }
  
  updateProfile(body:any){
    return this.http.post(this.apiUrl+'UPDATE_PROFILE/'+this.getUserIdFromSS(),body);
  }

  updateWholeInfo(body:any){
    return this.http.post(this.apiUrl+'UPDATE_NAME/'+this.getUserIdFromSS(),body);
  }

  getAllSaveData(){
    return this.http.get(this.apiUrl+'GET_SAVE_DATA/'+this.getUserIdFromSS());
  }

}
