private Rect[] NMS(Rect[] rects,float[] prob,float threshold = 0.3f) {
            var rect_ranges = new List<Rect>(rects);
            var rect_result = new List<Rect>();
            var keep_result = new bool[rects.Length];
       
            for(int idx = 0; idx < rects.Length - 1; idx++) {
                
                var order = rect_ranges.GetRange(idx + 1, (rects.Length - idx) - 1);
                var iou = Get_iou(order.ToArray(), rects[idx]);
                for(int col = 0; col < iou.Count; col++){
                    if(iou[col] > threshold) {
                        /* get rect */
                        keep_result[idx + col] = false;
                        //rect_result.RemoveAt(idx);
                    }
                    else {
                        keep_result[idx + col] = true;
                    }
                }
            }

            for(int idx = 0; idx < keep_result.Length; idx++) {
                if(keep_result[idx]) {
                    rect_result.Add(rect_ranges[idx]);
                }
            }

            return rect_result.ToArray();

        }
        private List<double> Get_iou(Rect[] rects,Rect box) {
            var rect_len = rects.Length;
            var xx1 = new List<double>();
            var yy1 = new List<double>();
            var union = new List<double>();

            for(int idx = 0; idx < rect_len; idx++) {
                xx1.Add(Math.Max(Math.Min(rects[idx].X + 0.5 * rects[idx].Width, box.X + 0.5 * box.Width) - Math.Max(rects[idx].X - 0.5 * rects[idx].Width, box.X - 0.5 * box.Width), 0));
                yy1.Add(Math.Max(Math.Min(rects[idx].Y + 0.5 * rects[idx].Height, box.Y + 0.5 * box.Height) - Math.Max(rects[idx].Y - 0.5 * rects[idx].Height, box.Y - 0.5 * box.Height), 0));
            }

            for(int idx = 0; idx < rect_len; idx++) {
                //inter.Add(xx1[idx] * yy1[idx]);
                var inter = (double)( xx1[idx] * yy1[idx] );
                union.Add(inter / (double)( ( rects[idx].Width * rects[idx].Height ) + ( box.Width * box.Height ) - inter ));
            }

            return union;
        }
