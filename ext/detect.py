
import cv2
import mediapipe as mp
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_hands = mp.solutions.hands

# For webcam input:
cap = cv2.VideoCapture(0)
with mp_hands.Hands(
    model_complexity=0,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5) as hands:
  while cap.isOpened():
    success, image = cap.read()
    if not success:
      print("Ignoring empty camera frame.")
      # If loading a video, use 'break' instead of 'continue'.
      continue

    # To improve performance, optionally mark the image as not writeable to
    # pass by reference.
    image.flags.writeable = False
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    results = hands.process(image)

    # Draw the hand annotations on the image.
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
    if results.multi_hand_landmarks:
      for hand_landmarks in results.multi_hand_landmarks:
        mp_drawing.draw_landmarks(
            image,
            hand_landmarks,
            mp_hands.HAND_CONNECTIONS,
            mp_drawing_styles.get_default_hand_landmarks_style(),
            mp_drawing_styles.get_default_hand_connections_style())

        # print('hand_landmarks:', hand_landmarks)
        # print(
        #     f'Thumb finger tip coordinates: (',
        #     f'{ int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].x * 1000) }, '
        #     f'{ int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].y * 1000) }, '
        #     f'{ int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].z * 1000) })'
        # )

        # vals = [
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].x * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].y * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].z * 100).to_bytes(4, 'big', signed=True),

        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].x * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].y * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].z * 100).to_bytes(4, 'big', signed=True),

        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP].x * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP].y * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP].z * 100).to_bytes(4, 'big', signed=True),

        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP].x * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP].y * 100).to_bytes(4, 'big', signed=True),
        #     int (hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP].z * 100).to_bytes(4, 'big', signed=True)
        # ]
        # 0, 4, 5, 17

        # print(vals);

        # f.write(bytearray(vals))

        try:
            f = open("C:\\Users\\srira\\source\\repos\\AICtrl\\ext\\data.bin", "wb")

            f.write(int (hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].x * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].y * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].z * 100).to_bytes(4, 'little', signed=True))

            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].x * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].y * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP].z * 100).to_bytes(4, 'little', signed=True))

            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP].x * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP].y * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP].z * 100).to_bytes(4, 'little', signed=True))

            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP].x * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP].y * 100).to_bytes(4, 'little', signed=True))
            f.write( int (hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP].z * 100).to_bytes(4, 'little', signed=True))
            
            f.close()
        except:

            pass

    # Flip the image horizontally for a selfie-view display.

    cv2.imshow('MediaPipe Hands', cv2.flip(image, 1))
    if cv2.waitKey(5) & 0xFF == 27:
      break
cap.release()