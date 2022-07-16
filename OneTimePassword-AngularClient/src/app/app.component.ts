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
    this.user = new User("1");
  }

  getOneTimePassword() {

  }

  title = 'OneTimePassword-Demo';

  submitted = false;
  async onSubmit() {
    await this.generateOneTimePasswordForUser();
    await this.updateValidOneTimePassword();
  }

  private async generateOneTimePasswordForUser() {
    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(this.user.id);
    await this.httpClient.post(this.baseURL + 'OneTimePassword/Generate', body, { 'headers': headers }).subscribe(result => {
      console.log(result);
    }, error => console.error(error));
  }

  private async updateValidOneTimePassword() {
    await this.httpClient.get<OneTimePassword>(this.baseURL + 'OneTimePassword/GetValidPassword?userId=' + this.user.id).subscribe(result => {
      var expirationTime = new Date(Date.now());
      expirationTime.setSeconds(expirationTime.getSeconds() + result.expirationInSeconds);
      this.user.oneTimePassword = new UserOneTimePassword(result.value, expirationTime);
    }, error => console.error(error));
  }
}

interface OneTimePassword {
  value: string;
  expirationInSeconds: number;
}

export class User {
  constructor(
    public id: string,
    public oneTimePassword?: UserOneTimePassword
  ) { }
}

export class UserOneTimePassword {
  public value: string;
  public expirationTime: Date;
  constructor(value: string, expirationTime: Date) {
    this.value = value;
    this.expirationTime = expirationTime;
  }
}
