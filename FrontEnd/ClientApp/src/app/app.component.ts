import { Component } from '@angular/core';
import { PostService } from './_services/post-service';
import { TopicService } from './_services/topic-service';
import { UserService } from './_services/user-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
    title = 'app';

    constructor(
        private postService: PostService,
        private topicService: TopicService,
        private userService: UserService) {

    }
    
}
