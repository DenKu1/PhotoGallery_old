import { Comment } from '../models/comment';

export class Photo {
  id: number;
  albumId: number;
  name: string;
  path: string;
  created: Date;
  likes: number;
  isLiked: boolean;
  comments: Comment[];
}
