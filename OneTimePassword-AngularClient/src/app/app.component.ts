import { HttpClient } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  public user: User;
  private httpClient: HttpClient;
  private baseURL: string = "https://localhost:7000/";

  constructor(http: HttpClient) {
    this.httpClient = http;
    this.user = new User("15", {
      value: "hello",
      validSeconds: 30
    });
  }

  getOneTimePassword() {

  }

  title = 'OneTimePassword-Demo';

  submitted = false;
  onSubmit() {
    this.submitted = true;


    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(this.user.id);
    this.httpClient.post(this.baseURL + 'OneTimePassword/Generate', body, { 'headers': headers }).subscribe(result => {
      console.log(result);
    }, error => console.error(error));


    this.httpClient.get<OneTimePassword>(this.baseURL + 'OneTimePassword/GetValidPassword?userId=' + this.user.id).subscribe(result => {
      this.user.oneTimePassword = result;
    }, error => console.error(error));
  }
}

interface OneTimePassword {
  value: string;
  validSeconds: number;
}

export class User {
  constructor(
    public id: string,
    public oneTimePassword: OneTimePassword
  ) { }
}
