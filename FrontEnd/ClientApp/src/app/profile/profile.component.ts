import { Component, OnInit } from '@angular/core';
import { PostService } from '../_services/post-service';
import { CommentService } from '../_services/comment-service';
import { CurrentUserService } from '../_services/currentUser-service';
import { NavbarFooterService } from '../_services/nav-service';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit{
    ngOnInit(): void {
        this.nav.show()
    }
    private user: IUser

    constructor(private currentUser: CurrentUserService,
        private postService: PostService,
        private commentService: CommentService,
        private nav: NavbarFooterService) {
        this.user = currentUser.get();
    }

    deletePost(id: number): void {
        this.postService.deletePost(id);
    }

    deleteComment(id: number, postId: number): void {
        this.commentService.deleteComment(id, postId);
    }
}
