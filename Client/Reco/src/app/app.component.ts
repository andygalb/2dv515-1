import {Component, OnInit} from '@angular/core';
import {DataService, Rating, Recommendation, User} from "./data.service";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  ngOnInit(): void {
    this.showUsers();
    this.showRatings(null);
  }

  title = 'Reco';
  users: User[];
  ratings: Rating[];
  recommendations: Recommendation[];
  currentUser: User;


  constructor(private dataService : DataService) {}

  showUsers() {

    this.dataService.getUsers()
      .subscribe((data: User[]) =>
      {
        console.log(data);
        this.users = data;
      });
  }

  showRatings(userID) {

    this.dataService.getRatings(userID)
      .subscribe((data: Rating[]) =>
      {
        console.log(data);
        this.ratings = data;
      });
  }


  showRatingsForCurrentUser() {

    this.dataService.getRatings(this.currentUser.userID)
      .subscribe((data: Rating[]) =>
      {
        console.log(data);
        this.ratings = data;
        this.showRecommendations(this.currentUser.userID);
      });

  }

  showRecommendations(userID) {

    this.dataService.getRecommendations(userID)
      .subscribe((data: Recommendation[]) =>
      {
        console.log("Recommendations:");
        console.log(data);
        this.recommendations = data;
      });
  }

  selectUser(user) {
    this.currentUser = user;
    console.log("Recommendations:");
    this.showRecommendations(this.currentUser.userID);
    this.showRatings(this.currentUser.userID);
  }

}

